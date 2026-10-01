import { useState } from 'react';
import type { SolveStep } from '../types';

interface StepViewerProps {
	steps: SolveStep[];
}

function formatCell(value: number): string {
	if (!Number.isFinite(value)) {
		return String(value);
	}

	const rounded = Math.round(value * 10000) / 10000;
	return Object.is(rounded, -0) ? '0' : String(rounded);
}

export function StepViewer({ steps }: StepViewerProps) {
	const [index, setIndex] = useState(0);
	const current = Math.min(index, steps.length - 1);
	const step = steps[current];
	const colCount = step.state[0]?.length ?? 0;

	return (
		<div className="card">
			<h2>Result</h2>

			<div className="step-nav">
				<button
					type="button"
					className="icon-btn"
					onClick={() => setIndex(current - 1)}
					disabled={current === 0}
					aria-label="Previous step"
				>
					&lsaquo;
				</button>
				<button
					type="button"
					className="icon-btn"
					onClick={() => setIndex(current + 1)}
					disabled={current === steps.length - 1}
					aria-label="Next step"
				>
					&rsaquo;
				</button>
				<span className="step-label">
					Step {current + 1} of {steps.length}
				</span>
			</div>

			<p className="step-description">{step.description}</p>

			<div className="matrix" style={{ gridTemplateColumns: `repeat(${colCount}, auto)` }}>
				{step.state.map((row, r) =>
					row.map((value, c) => (
						<div className="cell" key={`${r}-${c}`}>
							{formatCell(value)}
						</div>
					)),
				)}
			</div>
		</div>
	);
}
