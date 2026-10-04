import {
  type CheckoutProductSession,
} from '@/models/checkout/checkout.model'

export type CheckoutPropertyValues = Record<string, Record<string, unknown>>

export function validateCheckoutProperties(
  _sessions: CheckoutProductSession[],
  _values: CheckoutPropertyValues,
): Record<string, Record<string, string>> {
  // Product property rules removed — properties are optional at checkout.
  return {}
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
