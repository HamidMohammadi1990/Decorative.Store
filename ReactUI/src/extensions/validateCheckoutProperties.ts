import {
  PropertyType,
  type CheckoutProductSession,
  type CheckoutProperty,
  type CheckoutPropertyValues,
} from '@/models/checkout/checkout.model'

function isPropertyValueFilled(property: CheckoutProperty, value: unknown): boolean {
  if (value === undefined || value === null || value === '') return false

  switch (property.propertyType) {
    case PropertyType.Boolean:
      return true
    case PropertyType.Select:
      return typeof value === 'string' && value.length > 0
    case PropertyType.Text:
      return typeof value === 'string' && value.trim().length > 0
    case PropertyType.Numeric:
      return typeof value === 'number' && !Number.isNaN(value) && value > 0
    case PropertyType.NumericWithItem: {
      const current = value as { itemId?: string; quantity?: number }
      return Boolean(current.itemId) && (current.quantity ?? 0) > 0
    }
    case PropertyType.Dimensions: {
      const current = value as { width?: number; height?: number }
      return (current.width ?? 0) > 0 && (current.height ?? 0) > 0
    }
    case PropertyType.HasParents: {
      const current = value as { parentItemId?: string; nested?: Record<string, unknown> }
      if (!current.parentItemId) return false
      return property.parents.every((parent) => {
        if (!parent.rule?.isMandatory) return true
        return isPropertyValueFilled(parent, current.nested?.[parent.id])
      })
    }
    default:
      return true
  }
}

export function validateCheckoutProperties(
  sessions: CheckoutProductSession[],
  values: CheckoutPropertyValues,
): Record<string, Record<string, string>> {
  const errors: Record<string, Record<string, string>> = {}

  for (const session of sessions) {
    const productErrors: Record<string, string> = {}

    for (const group of session.data.properties) {
      for (const property of group.properties) {
        if (!property.rule?.isMandatory) continue
        const value = values[session.productId]?.[property.id]
        if (!isPropertyValueFilled(property, value)) {
          productErrors[property.id] = 'required'
        }
      }
    }

    if (Object.keys(productErrors).length > 0) {
      errors[session.productId] = productErrors
    }
  }

  return errors
}

function collectProperties(groups: CheckoutProductSession['data']['properties']) {
  return groups.flatMap((group) => group.properties)
}

export function flattenCheckoutProperties(sessions: CheckoutProductSession[]) {
  return sessions.flatMap((session) =>
    collectProperties(session.data.properties).map((property) => ({
      productId: session.productId,
      property,
    })),
  )
}
