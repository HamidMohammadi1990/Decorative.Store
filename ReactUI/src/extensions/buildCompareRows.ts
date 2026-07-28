import type { TFunction } from 'i18next'
import type { ProductDetail } from '@/models/catalog/productDetail.model'
import type { CompareRow } from '@/models/catalog/compare.model'
import { calcDiscountPercent } from '@/extensions/calcDiscountPercent'

function facetValues(
  product: ProductDetail,
  key: keyof ProductDetail['facets'],
  t: TFunction,
): string | null {
  const values = product.facets[key]
  if (!values?.length) return null
  return values.map((v) => t(`product.values.${v}`, { defaultValue: v })).join(', ')
}

function getFeatureValue(product: ProductDetail, label: string): string | null {
  const flat = product.features.find((f) => f.label === label)
  if (flat) return flat.value

  for (const group of product.featureGroups) {
    const match = group.features.find((f) => f.label === label)
    if (match) return match.value
  }

  return null
}

function collectFeatureLabels(products: ProductDetail[]): string[] {
  const labels = new Set<string>()
  for (const product of products) {
    product.features.forEach((f) => labels.add(f.label))
    product.featureGroups.forEach((g) =>
      g.features.forEach((f) => labels.add(f.label)),
    )
  }
  return Array.from(labels)
}

function valuesDiffer(values: (string | string[] | boolean | number | null)[]): boolean {
  const normalized = values.map((v) => {
    if (Array.isArray(v)) return v.join('|')
    if (typeof v === 'boolean') return v ? '1' : '0'
    if (typeof v === 'number') return String(v)
    return v ?? '—'
  })
  return new Set(normalized).size > 1
}

export function findBestPriceIndex(products: ProductDetail[]): number | null {
  if (products.length < 2) return null
  let bestIndex = 0
  let bestAmount = products[0].price.amount
  for (let i = 1; i < products.length; i += 1) {
    if (products[i].price.amount < bestAmount) {
      bestAmount = products[i].price.amount
      bestIndex = i
    }
  }
  const allSame = products.every((p) => p.price.amount === bestAmount)
  return allSame ? null : bestIndex
}

export function buildCompareRows(products: ProductDetail[], t: TFunction): CompareRow[] {
  const rows: CompareRow[] = []

  const priceValues = products.map((p) => p.price.amount)
  rows.push({ id: 'price', label: t('compare.rows.price'), type: 'price', values: priceValues })

  const discountValues = products.map((p) => {
    if (!p.compareAtPrice) return null
    const pct = calcDiscountPercent(p.price, p.compareAtPrice)
    return pct ? t('compare.discountValue', { percent: pct }) : null
  })
  if (discountValues.some(Boolean)) {
    rows.push({
      id: 'discount',
      label: t('compare.rows.discount'),
      type: 'text',
      values: discountValues,
    })
  }

  rows.push({
    id: 'availability',
    label: t('compare.rows.availability'),
    type: 'boolean',
    values: products.map((p) => p.inStock),
  })

  const staticRows: {
    id: string
    labelKey: string
    get: (p: ProductDetail) => string | null
  }[] = [
    { id: 'color', labelKey: 'compare.rows.color', get: (p) => facetValues(p, 'color', t) },
    { id: 'size', labelKey: 'compare.rows.size', get: (p) => facetValues(p, 'size', t) },
    {
      id: 'material',
      labelKey: 'compare.rows.material',
      get: (p) => facetValues(p, 'material', t),
    },
    { id: 'room', labelKey: 'compare.rows.room', get: (p) => facetValues(p, 'room', t) },
    {
      id: 'dimensions',
      labelKey: 'compare.rows.dimensions',
      get: (p) => p.dimensions ?? null,
    },
    { id: 'warranty', labelKey: 'compare.rows.warranty', get: (p) => p.warranty ?? null },
    { id: 'care', labelKey: 'compare.rows.care', get: (p) => p.care ?? null },
    {
      id: 'delivery',
      labelKey: 'compare.rows.delivery',
      get: (p) => p.deliveryNote ?? null,
    },
  ]

  for (const row of staticRows) {
    const values = products.map((p) => row.get(p))
    if (values.some(Boolean)) {
      rows.push({
        id: row.id,
        label: t(row.labelKey),
        type: 'text',
        values,
      })
    }
  }

  const highlightValues = products.map((p) => p.highlights)
  if (highlightValues.some((h) => h.length > 0)) {
    rows.push({
      id: 'highlights',
      label: t('compare.rows.highlights'),
      type: 'list',
      values: highlightValues,
    })
  }

  for (const label of collectFeatureLabels(products)) {
    const values = products.map((p) => getFeatureValue(p, label))
    if (values.some(Boolean)) {
      rows.push({
        id: `feature-${label}`,
        label,
        type: 'text',
        values,
      })
    }
  }

  return rows.map((row) => ({
    ...row,
    highlightDiff: valuesDiffer(row.values),
  }))
}
