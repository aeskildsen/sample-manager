import React, { useRef, useCallback, useState } from 'react'
import { useWavesurfer } from '@wavesurfer/react'
import { FaPlay, FaPause, FaVolumeUp, FaVolumeDown, FaVolumeMute } from 'react-icons/fa'
import styles from './AudioPlayer.module.css'

export interface AudioPlayerProps {
  url: string | null
  height?: number
  waveColor?: string
  progressColor?: string
  backgroundColor?: string
  showControls?: boolean
  showTime?: boolean
  showVolume?: boolean
  className?: string
}

export const AudioPlayer: React.FC<AudioPlayerProps> = ({
  url,
  height = 80,
  waveColor,
  progressColor,
  showControls = true,
  showTime = true,
  showVolume = true,
  className = '',
}) => {
  
  if (!url) return <p>No file loaded.</p>
  
  const containerRef = useRef<HTMLDivElement>(null)
  const [volume, setVolume] = useState(0.7)
  const [isMuted, setIsMuted] = useState(false)
  const [previousVolume, setPreviousVolume] = useState(0.7)
  
  const getThemeColor = (property: string, fallback: string) => {
    const computedStyle = window.getComputedStyle(document.documentElement)
    return computedStyle.getPropertyValue(property).trim() || fallback
  }
  
  const finalWaveColor = waveColor || getThemeColor('--accent-primary', 'goldenrod')
  const finalProgressColor = progressColor || getThemeColor('--accent-secondary', 'orange')
  
  
  const { wavesurfer, isReady, isPlaying, currentTime } = useWavesurfer({
    container: containerRef,
    url,
    waveColor: finalWaveColor,
    progressColor: finalProgressColor,
    height,
    barWidth: 2,
    barRadius: 3,
    barGap: 2,
    normalize: true,
    dragToSeek: true,
  })
  
  const handlePlayPause = useCallback(() => {
    if (wavesurfer && isReady) {
      wavesurfer.playPause()
    }
  }, [wavesurfer, isReady])

  const handleVolumeChange = useCallback((event: React.ChangeEvent<HTMLInputElement>) => {
    if (wavesurfer) {
      const newVolume = parseFloat(event.target.value)
      setVolume(newVolume)
      wavesurfer.setVolume(newVolume)
      if (newVolume === 0) {
        setIsMuted(true)
      } else if (isMuted) {
        setIsMuted(false)
      }
    }
  }, [wavesurfer, isMuted])

  const handleMuteToggle = useCallback(() => {
    if (wavesurfer) {
      if (isMuted) {
        const restoreVolume = previousVolume > 0 ? previousVolume : 0.7
        setVolume(restoreVolume)
        wavesurfer.setVolume(restoreVolume)
        setIsMuted(false)
      } else {
        setPreviousVolume(volume)
        setVolume(0)
        wavesurfer.setVolume(0)
        setIsMuted(true)
      }
    }
  }, [wavesurfer, isMuted, volume, previousVolume])

  const getVolumeIcon = () => {
    if (isMuted || volume === 0) return <FaVolumeMute />
    if (volume < 0.5) return <FaVolumeDown />
    return <FaVolumeUp />
  }

  const formatTime = (time: number) => {
    const minutes = Math.floor(time / 60)
    const seconds = Math.floor(time % 60)
    return `${minutes}:${seconds.toString().padStart(2, '0')}`
  }

  const duration = wavesurfer?.getDuration() || 0

  return (
    <div className={`${styles.audioPlayer} ${className}`}>
      <div
        ref={containerRef}
        className={styles.waveformContainer}
        style={{ opacity: isReady ? 1 : 0.5 }}
      />
      {!isReady && (
        <div className={styles.loadingOverlay}>
            <div className={styles.loadingSpinner} />
            <span>Loading audio...</span>
        </div>
      )}
      {showControls && isReady && (
        <div className={styles.controls}>
          <button
            onClick={handlePlayPause}
            className={styles.playButton}
            disabled={!isReady}
            aria-label={isPlaying ? 'Pause' : 'Play'}
          >
            {isPlaying ? <FaPause /> : <FaPlay />}
          </button>
          {showTime && (
            <div className={styles.timeDisplay}>
              {formatTime(currentTime)} / {formatTime(duration)}
            </div>
          )}
          {showVolume && (
            <div className={styles.volumeControl}>
              <button
                onClick={handleMuteToggle}
                className={styles.muteButton}
                aria-label={isMuted ? 'Unmute' : 'Mute'}
              >
                {getVolumeIcon()}
              </button>
              <input
                type="range"
                min="0"
                max="1"
                step="0.1"
                value={volume}
                onChange={handleVolumeChange}
                className={styles.volumeSlider}
                aria-label="Volume"
              />
            </div>
          )}
        </div>
      )}
    </div>
  )
}

export default AudioPlayer
