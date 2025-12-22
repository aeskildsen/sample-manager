import type { selectionHandler } from "../types/ui"
import './BreadCrumbs.css'

interface BreadCrumbsProps {
  path: string | null,
  filters: string[] | null,
  handleLocationChange: selectionHandler
}

export function BreadCrumbs({
  path,
  filters,
  handleLocationChange
}: BreadCrumbsProps) {
  const folders = path ? path.split('/') : ['']

  const crumbs = folders
    .map((folder, index) => {
      const path = [...folders.slice(0, index + 1)].join('/')
      if (path == '') return ''
      return (
        <li className="crumb" key={index}>
          <button onClick={() => handleLocationChange(path)}>
            {folder}
          </button>
        </li>
      )
    })

  return (
    <ul className="panel breadcrumbs">
      {crumbs}
      {filters}
    </ul>
  )
}