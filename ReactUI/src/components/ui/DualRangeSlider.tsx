import { useCallback, useEffect, useRef, useState } from 'react'

interface DualRangeSliderProps {
  min: number
  max: number
  step?: number
  valueMin: number
  valueMax: number
  onChange: (min: number, max: number) => void
  onCommit?: (min: number, max: number) => void
  ariaLabelMin: string
  ariaLabelMax: string
}

export function DualRangeSlider({
  min,
  max,
  step = 1,
  valueMin,
  valueMax,
  onChange,
  onCommit,
  ariaLabelMin,
  ariaLabelMax,
}: DualRangeSliderProps) {
  const [activeThumb, setActiveThumb] = useState<'min' | 'max' | null>(null)
  const commitRef = useRef(onCommit)
  const latestRef = useRef({ min: valueMin, max: valueMax })
  commitRef.current = onCommit

  useEffect(() => {
    latestRef.current = { min: valueMin, max: valueMax }
  }, [valueMin, valueMax])

  const range = Math.max(max - min, 1)
  const minPercent = ((valueMin - min) / range) * 100
  const maxPercent = ((valueMax - min) / range) * 100

  const commit = useCallback(
    (nextMin: number, nextMax: number) => {
      commitRef.current?.(nextMin, nextMax)
    },
    [],
  )

  const minOnTop =
    activeThumb === 'min' || (activeThumb == null && valueMin > max - range * 0.5)

  if (min >= max) return null

  return (
    <div className="relative pb-1 pt-1">
      <div className="relative mx-2 h-7">
        <div className="absolute inset-x-0 top-1/2 h-1.5 -translate-y-1/2 rounded-full bg-border" />
        <div
          className="absolute top-1/2 h-1.5 -translate-y-1/2 rounded-full bg-warm"
          style={{
            insetInlineStart: `${minPercent}%`,
            width: `${Math.max(0, maxPercent - minPercent)}%`,
          }}
        />

        <input
          type="range"
          min={min}
          max={max}
          step={step}
          value={valueMin}
          aria-label={ariaLabelMin}
          onChange={(e) => {
            const nextMin = Math.min(Number(e.target.value), valueMax)
            latestRef.current = { min: nextMin, max: valueMax }
            onChange(nextMin, valueMax)
          }}
          onPointerDown={() => setActiveThumb('min')}
          onPointerUp={() => {
            setActiveThumb(null)
            commit(latestRef.current.min, latestRef.current.max)
          }}
          onKeyUp={() => commit(latestRef.current.min, latestRef.current.max)}
          className={`dual-range-input ${minOnTop ? 'z-20' : 'z-10'}`}
        />

        <input
          type="range"
          min={min}
          max={max}
          step={step}
          value={valueMax}
          aria-label={ariaLabelMax}
          onChange={(e) => {
            const nextMax = Math.max(Number(e.target.value), valueMin)
            latestRef.current = { min: valueMin, max: nextMax }
            onChange(valueMin, nextMax)
          }}
          onPointerDown={() => setActiveThumb('max')}
          onPointerUp={() => {
            setActiveThumb(null)
            commit(latestRef.current.min, latestRef.current.max)
          }}
          onKeyUp={() => commit(latestRef.current.min, latestRef.current.max)}
          className={`dual-range-input ${minOnTop ? 'z-10' : 'z-20'}`}
        />
      </div>
    </div>
  )
}
