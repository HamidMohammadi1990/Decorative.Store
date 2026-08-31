import { RouterProvider } from 'react-router-dom'
import { ConfirmProvider } from '@/components/ui/ConfirmProvider'
import { router } from '@/routes'

function App() {
  return (
    <ConfirmProvider>
      <RouterProvider router={router} />
    </ConfirmProvider>
  )
}

export default App
