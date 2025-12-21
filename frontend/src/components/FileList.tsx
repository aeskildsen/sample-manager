import './FileList.css'
import type { FolderContents } from "../types/api";
import type { fileSelectionHandler } from '../types/ui';
import { formatTime } from '../utils/durations';

interface FileListProps {
  folderContents: FolderContents | null,
  loading: boolean,
  handleFileSelection: fileSelectionHandler
}

export default function FileList({
  folderContents,
  loading,
  handleFileSelection
}: FileListProps) {
  if (loading || !folderContents) {
    return <main className='file-list'></main>
  }

  const files = folderContents.audioFiles.sort((a, b) => a.name.toLowerCase() > b.name.toLowerCase() ? 1 : -1)

  const list = files.map(file => (
    <tr key={file.filePath}>
      <td className="name" onClick={() => handleFileSelection(file)}>{file.name}</td>
      <td>{file.format}</td>
      <td className="duration">{formatTime(file.duration)}</td>
      <td>{file.channels}</td>
      <td>{file.packSlug}</td>
      <td>{file.licenseSlug}</td>
    </tr>
  ))

  return (
    <main className='file-list panel'>
      <table>
        <thead>
          <tr>
            <td className="name">Name</td>
            <td>Format</td>
            <td className="duration">Duration</td>
            <td>Channels</td>
            <td>Pack</td>
            <td>License</td>
          </tr>
        </thead>
        <tbody>
          {list}
        </tbody>
      </table>
    </main>
  )
}