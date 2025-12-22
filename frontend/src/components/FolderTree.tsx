import './FolderTree.css'
import type { FolderNode } from '../types/api'
import type { selectionHandler } from '../types/ui';
import { MdKeyboardArrowRight, MdKeyboardArrowDown } from "react-icons/md";
import { useState } from 'react'

interface FolderTreeProps {
  folderTree: FolderNode | null,
  loading: boolean,
  handleFolderSelection: selectionHandler,
}

interface TreeNodeProps extends FolderNode {
  handleFolderSelection: selectionHandler
}


export default function FolderTree({ folderTree, loading, handleFolderSelection }: FolderTreeProps){
  
  if (loading || !folderTree) {
    return <aside className="folder-tree"></aside>
  }

  const topFolders = folderTree.subFolders

  topFolders.sort((a, b) => a.name > b.name ? 1 : -1)

  const list = topFolders.map(folder => (
    <TreeNode
      { ...folder }
      key={folder.path}
      handleFolderSelection={handleFolderSelection}
    />
  ))

  return (
    <nav className='folder-tree panel'>
      {list}
    </nav>
  )
}

function TreeNode({name, path, subFolders, handleFolderSelection}: TreeNodeProps ){
  const [isExpanded, setIsExpanded] = useState(false)
  const hasSubfolders = subFolders.length > 0 
  return (
    <li className="folder-node">
      { !hasSubfolders ?
      <span className="inactive"><MdKeyboardArrowRight /></span> :
      <span onClick={() => setIsExpanded(previous => !previous)}>
        { isExpanded
        ? <MdKeyboardArrowDown />
        : <MdKeyboardArrowRight />
        }
      </span>
      }
      <button onClick={() => {
        handleFolderSelection(path)
        setIsExpanded(previous => !previous)
        }}>{ name }</button>
      
      { hasSubfolders && isExpanded &&
      <ul>
        {subFolders.map(subFolder => (
          <TreeNode
            {...subFolder}
            key={subFolder.path}
            handleFolderSelection={handleFolderSelection}
          />
        ) 
      )}
      </ul>}
    </li>
  )
}