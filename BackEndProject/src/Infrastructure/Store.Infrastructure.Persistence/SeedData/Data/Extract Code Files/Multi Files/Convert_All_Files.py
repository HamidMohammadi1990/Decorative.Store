#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import os
import json
from bs4 import BeautifulSoup
from extract_html_to_json_script import parse_html_to_structure  # اگر اسم فایل قبلی‌ات فرق دارد، اصلاح کن

INPUT_DIR = "Contents"
OUTPUT_DIR = "Properties"

def process_all_files():
    if not os.path.exists(INPUT_DIR):
        print(f"❌ Input directory not found: {INPUT_DIR}")
        return
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    files = [f for f in os.listdir(INPUT_DIR) if f.lower().endswith(".html")]
    if not files:
        print("❌ No .html files found in Contents/")
        return

    for filename in files:
        input_path = os.path.join(INPUT_DIR, filename)
        output_name = os.path.splitext(filename)[0] + ".json"
        output_path = os.path.join(OUTPUT_DIR, output_name)

        with open(input_path, "r", encoding="utf-8") as f:
            html = f.read()

        structure = parse_html_to_structure(html)

        with open(output_path, "w", encoding="utf-8") as f:
            json.dump(structure, f, ensure_ascii=False, indent=2)

        print(f"✅ Converted {filename} → {output_name}")

if __name__ == "__main__":
    process_all_files()