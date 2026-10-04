const MOCK_LATENCY_MS = 120

export async function mockFetch<T>(loader: () => Promise<T>): Promise<T> {
  await new Promise((resolve) => setTimeout(resolve, MOCK_LATENCY_MS))
  return loader()
}
