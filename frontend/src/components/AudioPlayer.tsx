import React, { useRef, useState, useEffect } from 'react'
import { useWavesurfer } from '@wavesurfer/react'
import { FaPlay, FaPause, FaStop } from 'react-icons/fa'
import { ImLoop } from "react-icons/im";
// import styles from './AudioPlayer.module.css'
import './AudioPlayer.css'
import type { AudioFile } from '../types/api'
import { formatTime } from '../utils/durations'

export interface AudioPlayerProps {
  url: string | null
  height?: number
  waveColor?: string
  progressColor?: string
  backgroundColor?: string
  showControls?: boolean
  showTime?: boolean
  showVolume?: boolean
  metadata: AudioFile
}

export const AudioPlayer: React.FC<AudioPlayerProps> = ({
  url,
  metadata,
  height = 130,
  waveColor = 'green',
  progressColor = 'green',
}) => {
  const containerRef = useRef<HTMLDivElement>(null)
  const [volume, setVolume] = useState(0.7)
  const [isLooping, setIsLooping] = useState(false)
  //const [isMuted, setIsMuted] = useState(false)
  
  const { wavesurfer, isReady, isPlaying, currentTime } = useWavesurfer({
    container: containerRef,
    url: url || '',
    waveColor,
    progressColor,
    height,
    barWidth: 2,
    barRadius: 1,
    barGap: 1,
    normalize: true,
    dragToSeek: true,
  })
  /*
  const getVolumeIcon = () => {
    if (isMuted || volume === 0) return <FaVolumeMute />
    if (volume < 0.5) return <FaVolumeDown />
    return <FaVolumeUp />
  }
    */
  const duration = wavesurfer?.getDuration() || 0
  
  const handlePlayPause = () => {
    wavesurfer?.playPause();
  }

  const handleStop = () => {
    wavesurfer?.stop();
  }

  const handleVolumeChange = (ev: React.ChangeEvent<HTMLInputElement>) => {
    const newVolume = parseFloat(ev.target.value)
    wavesurfer?.setVolume(newVolume)
    setVolume(newVolume)
  }

  useEffect(() => {
    if (!wavesurfer) return
    const handleFinish = () => {
      if (isLooping) {
        wavesurfer.play()
      }
    }
    const unsubscribe = wavesurfer.on('finish', handleFinish)
    return unsubscribe
  }, [wavesurfer, isLooping])

  return (
    <div className='player panel'>
      
      <div className='controls'>
        <div className='name'>{metadata ? metadata.name : ''}</div>
        <div className='clock'>
          {formatTime(currentTime)} / {formatTime(duration)}
        </div>
        <div className='control-row'>
          <input type="range" min="0" max="1" step="0.01" value={volume} onChange={handleVolumeChange}/>
        </div>
        <div className='control-row'>
          <button onClick={handlePlayPause}>
          { isPlaying ? <FaPause /> : <FaPlay /> }
          </button>
          <button onClick={handleStop}>
            <FaStop />
          </button>
          <button 
            className={isLooping ? 'active' : ''}
            onClick={() => setIsLooping(prev => !prev)}
            >
            <ImLoop />
          </button>
        </div>
      </div>

      <div className='waveform-container'>
        <div ref={containerRef} />
        {(!url || !isReady ) && <p className="placeholder">No file selected.</p>}
      </div>

      <div className='metadata'>
        <table>
          <tr><td>Samplerate:</td><tr>{metadata.sampleRate}</tr></tr>
          <tr><td>Bit depth:</td><tr>{metadata.bitDepth}</tr></tr>
          <tr><td>Channels:</td><tr>{metadata.channels}</tr></tr>
        </table>
      </div>
    </div>
  )
}

export default AudioPlayer
