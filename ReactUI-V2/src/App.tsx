import { RouterProvider } from 'react-router-dom'
import { createAppRouter } from '@/routes/createAppRouter'

interface AppProps {
  router: ReturnType<typeof createAppRouter>
}

function App({ router }: AppProps) {
  return <RouterProvider router={router} />
}

export default App
