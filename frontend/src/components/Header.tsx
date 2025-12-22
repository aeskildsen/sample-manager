
import './Header.css'

interface HeaderProps {
  errors: (Error | null)[]
}

export default function Header({errors}: HeaderProps) {
  
  return (
    <header className='header panel'>
        <h1>Sample manager</h1>
        { errors.map(error => error ? <span>{error.message}</span> : '')}
        <button>Settings</button>
    </header>
  )
}