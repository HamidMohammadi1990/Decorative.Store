import type { Router } from 'react-router-dom'
import { RouterProvider } from 'react-router-dom'

interface AppProps {
  router: Router
}

function App({ router }: AppProps) {
  return <RouterProvider router={router} />
}

export default App
