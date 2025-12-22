import './Chip.css'

interface ChipProps {
  clickHandler: () => void,
  children: React.ReactNode,
  className?: string
}

export default function Chip({
  clickHandler,
  children,
  className
}: ChipProps) {
  return (
    <span
      onClick={clickHandler}
      className={'chip ' + (className ? className: '')}>
      {children}
    </span>
  )
}