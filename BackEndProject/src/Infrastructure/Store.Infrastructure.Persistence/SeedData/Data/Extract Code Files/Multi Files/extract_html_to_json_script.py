#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from bs4 import BeautifulSoup
import re, json, sys, os

def is_element_visible(el):
    if el is None:
        return False
    if el.name == "input" and el.get("type", "").lower() == "hidden":
        return False
    cls = " ".join(el.get("class") or [])
    if "d-none" in cls or "hidden" in cls:
        return False
    for p in el.parents:
        pcl = " ".join(p.get("class") or [])
        if "d-none" in pcl or "hidden" in pcl:
            return False
    return True

def get_label_text_for(el):
    if el is None:
        return ""
    prev_label = el.find_previous(lambda tag: tag.name=="label")
    if prev_label and prev_label.text and prev_label.text.strip():
        return prev_label.text.strip()
    def parent_match(tag):
        if tag.name != "div":
            return False
        cls = " ".join(tag.get("class") or [])
        return ("row" in cls) or ("line" in cls) or (tag.get("id") == "coverBox")
    row = el.find_parent(parent_match)
    if row:
        label_in_row = row.find("label")
        if label_in_row and label_in_row.text and label_in_row.text.strip():
            return label_in_row.text.strip()
    box = el.find_parent("div", class_="box-custom")
    if box:
        h3 = box.find("h3", class_="title")
        if h3 and h3.text and h3.text.strip():
            return h3.text.strip()
    ph = el.get("placeholder") or el.get("aria-label") or el.get("name") or el.get("id") or ""
    return str(ph).strip()

def is_required(el):
    if el is None:
        return False
    if el.has_attr("required"):
        return True
    if str(el.get("aria-required","")).lower() == "true":
        return True
    if el.has_attr("data-val-required"):
        return True
    return False

def extract_items_from_select(sel):
    items = []
    for opt in sel.find_all("option"):
        val = opt.get("value")
        title = (opt.text or "").strip()
        if val is None:
            continue
        if val.strip() == "" or val.strip().lower() == "null":
            continue
        items.append({"Id": str(val).strip(), "Title": title})
    return items

def extract_select_property(sel):
    return {
        "Title": get_label_text_for(sel),
        "Type": "select",
        "IsRequired": bool(is_required(sel)),
        "Min": 0,
        "Max": 0,
        "Items": extract_items_from_select(sel),
        "Dependencies": [],
        "Properties": []
    }

def extract_text_input_property(inp, label_override=None, for_dependency=False):
    
    if for_dependency and inp.has_attr("placeholder") and inp["placeholder"].strip():
        label = inp["placeholder"].strip()
    else:
        label = label_override if label_override is not None else get_label_text_for(inp)

    t = inp.get("type", "text")
    classes = inp.get("class") or []
    if "input-number" in classes:
        t = "number"
    try:
        minv = int(inp.get("min")) if inp.get("min") is not None and inp.get("min") != "" else 0
    except:
        minv = 0
    try:
        maxv = int(inp.get("max")) if inp.get("max") is not None and inp.get("max") != "" else 0
    except:
        maxv = 0
    return {
        "Title": label,
        "Type": t if t else "text",
        "IsRequired": bool(is_required(inp)),
        "Min": minv,
        "Max": maxv,
        "Items": [],
        "Dependencies": [],
        "Properties": []
    }

def extract_checkbox_property(inp):
    return {
        "Title": get_label_text_for(inp),
        "Type": "checkbox",
        "IsRequired": bool(is_required(inp)),
        "Min": 0,
        "Max": 0,
        "Items": [],
        "Dependencies": [],
        "Properties": []
    }

def coalesce_label_text(s):
    return (s or "").strip()

def group_elements_by_suffix_or_prefix(elements):
    suffix_groups = {}
    no_suffix = []
    for i, e in enumerate(elements):
        eid = e.get("id") or ""
        m = re.search(r"(\d+)$", eid)
        if m:
            key = "n:" + m.group(1)
            suffix_groups.setdefault(key, []).append(i)
        else:
            no_suffix.append(i)
    groups = [v for v in suffix_groups.values() if len(v)>0]
    prefix_map = {}
    for i in no_suffix:
        eid = elements[i].get("id") or ""
        m = re.match(r"^([a-zA-Z_]+)", eid)
        key = m.group(1) if m else eid[:4]
        prefix_map.setdefault(key, []).append(i)
    for k, v in prefix_map.items():
        if len(v) > 1:
            groups.append(v)
    assigned = set(sum(groups, [])) if groups else set()
    for i in range(len(elements)):
        if i not in assigned:
            groups.append([i])
    return groups

# --------- core: process a single box-custom (but do NOT create top-level category for nested boxes) -----------
def process_box(box):
    """
    Processes one box-custom and returns a dict:
      {"CategoryTitle": "<title>", "Properties": [ ... ]}
    This function will NOT treat nested box-custom as top-level; nested ones are returned in Properties as requested.
    """
    cat_title = coalesce_label_text(box.find("h3", class_="title").get_text(strip=True) if box.find("h3", class_="title") else "Unknown")
    category = {"CategoryTitle": cat_title, "Properties": []}

    # collect elements that belong to THIS box (i.e., whose closest ancestor box-custom is this box)
    elements = []
    for sel in box.find_all("select"):
        if not is_element_visible(sel):
            continue
        # ensure this select belongs to this box (its nearest box-custom parent is this box)
        nearest_box = sel.find_parent("div", class_="box-custom")
        if nearest_box is not box:
            continue
        elements.append({"el": sel, "id": sel.get("id") or sel.get("name") or "", "tag": "select", "processed": False})

    for inp in box.find_all("input"):
        if not is_element_visible(inp):
            continue
        if inp.get("type","").lower() == "hidden":
            continue
        nearest_box = inp.find_parent("div", class_="box-custom")
        if nearest_box is not box:
            continue
        elements.append({"el": inp, "id": inp.get("id") or inp.get("name") or "", "tag": "input", "type": inp.get("type","text").lower(), "processed": False})

    # 1) custom-control blocks that belong to this box
    for custom in box.find_all("div", class_="custom-control"):
        if not is_element_visible(custom):
            continue
        nearest_box = custom.find_parent("div", class_="box-custom")
        if nearest_box is not box:
            continue
        cb = custom.find("input", {"type": "checkbox"})
        if not cb or not is_element_visible(cb):
            continue
        lbl = custom.find("label")
        main_title = lbl.get_text(strip=True) if lbl and lbl.text else get_label_text_for(cb)
        main_prop = {
            "Title": main_title,
            "Type": "checkbox",
            "IsRequired": bool(is_required(cb)),
            "Min": 0,
            "Max": 0,
            "Items": [],
            "Dependencies": [],
            "Properties": []
        }
        parent_row = custom.find_parent("div", class_="row")
        if not parent_row:
            parent_row = custom.parent
        deps = []
        if parent_row:
            for dep in parent_row.find_all(["select","input"]):
                if dep == cb:
                    continue
                if not is_element_visible(dep):
                    continue
                # ensure dependency also belongs to this box (avoid grabbing nested-box controls)
                nearest_box_dep = dep.find_parent("div", class_="box-custom")
                if nearest_box_dep is not box:
                    continue
                if dep.name == "select":
                    dprop = extract_select_property(dep)
                    deps.append(dprop)
                    for e in elements:
                        if e["el"] == dep:
                            e["processed"] = True
                elif dep.name == "input" and dep.get("type","").lower() != "checkbox":
                    dprop = extract_text_input_property(dep, for_dependency= True)
                    deps.append(dprop)
                    for e in elements:
                        if e["el"] == dep:
                            e["processed"] = True
        main_prop["Dependencies"] = deps
        category["Properties"].append(main_prop)
        for e in elements:
            if e["el"] == cb:
                e["processed"] = True

    # 2) remaining elements grouping (same logic as before)
    remaining = [e for e in elements if not e.get("processed", False)]
    groups = group_elements_by_suffix_or_prefix(remaining)

    for grp in groups:
        if len(grp) == 0:
            continue
        group_elems = [remaining[i] for i in grp]
        main_idx = None
        # prefer checkbox (should be handled above, but keep check)
        for i, ge in enumerate(group_elems):
            el = ge["el"]
            if el.name == "input" and ge.get("type") == "checkbox":
                main_idx = i
                break
        if main_idx is None:
            for i, ge in enumerate(group_elems):
                if (ge.get("id") or "").lower().endswith("id"):
                    main_idx = i
                    break
        if main_idx is None:
            for i, ge in enumerate(group_elems):
                if ge["tag"] == "select":
                    main_idx = i
                    break
        if main_idx is None:
            main_idx = 0

        main_elem = group_elems[main_idx]["el"]
        if main_elem.name == "select":
            main_prop = extract_select_property(main_elem)
        elif main_elem.name == "input":
            label_text = get_label_text_for(main_elem)
            text_inputs_in_group = [ge["el"] for ge in group_elems if ge["el"].name=="input" and ge["el"].get("type","").lower()!="checkbox"]
            if len(text_inputs_in_group) > 1 and label_text:
                mins = []
                maxs = []
                req = False
                for ti in text_inputs_in_group:
                    try:
                        if ti.get("min") is not None and ti.get("min") != "":
                            mins.append(int(ti.get("min")))
                    except:
                        pass
                    try:
                        if ti.get("max") is not None and ti.get("max") != "":
                            maxs.append(int(ti.get("max")))
                    except:
                        pass
                    if is_required(ti):
                        req = True
                minv = min(mins) if mins else 0
                maxv = max(maxs) if maxs else 0
                main_prop = {
                    "Title": label_text,
                    "Type": "text",
                    "IsRequired": req,
                    "Min": minv,
                    "Max": maxv,
                    "Items": [],
                    "Dependencies": [],
                    "Properties": []
                }
                for ge in group_elems:
                    for e in elements:
                        if e["el"] == ge["el"]:
                            e["processed"] = True
                category["Properties"].append(main_prop)
                continue
            else:
                main_prop = extract_text_input_property(main_elem)

        deps = []
        for i, ge in enumerate(group_elems):
            if i == main_idx:
                continue
            el = ge["el"]
            if el.name == "select":
                deps.append(extract_select_property(el))
            elif el.name == "input" and el.get("type","").lower() != "checkbox":
                deps.append(extract_text_input_property(el))
            for e in elements:
                if e["el"] == el:
                    e["processed"] = True
        main_prop["Dependencies"] = deps if deps else []
        category["Properties"].append(main_prop)
        for e in elements:
            if e["el"] == main_elem:
                e["processed"] = True

    # 3) leftover unprocessed elements -> standalone properties
    for e in elements:
        if e.get("processed"):
            continue
        el = e["el"]
        if el.name == "select":
            category["Properties"].append(extract_select_property(el))
        elif el.name == "input" and e.get("type") != "checkbox":
            category["Properties"].append(extract_text_input_property(el))
        elif el.name == "input" and e.get("type") == "checkbox":
            category["Properties"].append(extract_checkbox_property(el))

    # 4) process nested box-customs that are direct children of this box (i.e., their nearest box-custom parent is this box)
    for inner in box.find_all("div", class_="box-custom"):
        if inner is box:
            continue
        nearest = inner.find_parent("div", class_="box-custom")
        if nearest is not box:
            continue
        nested_cat = process_box(inner)  # {"CategoryTitle":..., "Properties":[...]}

        nested_prop = {
            "Title": nested_cat["CategoryTitle"],
            "InnerProperties": [
                {
                    "Title": nested_cat["CategoryTitle"],
                    "Properties": nested_cat["Properties"]
                }
            ]
        }
        category["Properties"].append(nested_prop)

    return category

# ---------- parse whole html ----------
def parse_html_to_structure(html_text):
    soup = BeautifulSoup(html_text, "html.parser")
    categories = []

    # General
    general = {"CategoryTitle": "General", "Properties": []}
    select_data = soup.find("div", class_="select_data")
    if select_data:
        lines = select_data.find_all("div", class_="line", recursive=True)
        for line in lines:
            if not is_element_visible(line):
                continue
            title_input = line.find("input", {"id": "title"})
            if title_input:
                continue
            text_inputs = [inp for inp in line.find_all("input", {"type": "text"}) if is_element_visible(inp)]
            selects = [sel for sel in line.find_all("select") if is_element_visible(sel)]
            label_tag = line.find("label")
            label_text = label_tag.text.strip() if label_tag and label_tag.text else ""
            if len(text_inputs) > 1 and label_text:
                mins = []
                maxs = []
                req = False
                for inp in text_inputs:
                    try:
                        if inp.get("min") is not None and inp.get("min") != "":
                            mins.append(int(inp.get("min")))
                    except:
                        pass
                    try:
                        if inp.get("max") is not None and inp.get("max") != "":
                            maxs.append(int(inp.get("max")))
                    except:
                        pass
                    if is_required(inp):
                        req = True
                minv = min(mins) if mins else 0
                maxv = max(maxs) if maxs else 0
                prop = {
                    "Title": label_text,
                    "Type": "text",
                    "IsRequired": req,
                    "Min": minv,
                    "Max": maxv,
                    "Items": [],
                    "Dependencies": [],
                    "Properties": []
                }
                general["Properties"].append(prop)
                continue
            for sel in selects:
                general["Properties"].append(extract_select_property(sel))
            for inp in text_inputs:
                if inp.get("id") == "title":
                    continue
                general["Properties"].append(extract_text_input_property(inp))
    categories.append(general)

    # only top-level box-customs (whose nearest box-custom parent is None)
    all_boxes = soup.find_all("div", class_="box-custom")
    top_level_boxes = []
    for b in all_boxes:
        if b.find_parent("div", class_="box-custom") is None:
            top_level_boxes.append(b)

    for box in top_level_boxes:
        cat = process_box(box)
        categories.append(cat)

    return categories

def main():
    if len(sys.argv) >= 2:
        input_path = sys.argv[1]
    else:
        input_path = "input.html"
    if len(sys.argv) >= 3:
        output_path = sys.argv[2]
    else:
        output_path = "output.json"

    if not os.path.exists(input_path):
        print("Input file not found:", input_path)
        return

    with open(input_path, "r", encoding="utf-8") as f:
        html = f.read()

    structure = parse_html_to_structure(html)

    with open(output_path, "w", encoding="utf-8") as f:
        json.dump(structure, f, ensure_ascii=False, indent=2)

    print("Wrote:", output_path)

if __name__ == "__main__":
    main()
