interface StepperProps {
	label: string;
	value: number;
	min: number;
	max: number;
	onChange: (value: number) => void;
}

export function Stepper({ label, value, min, max, onChange }: StepperProps) {
	return (
		<div className="field">
			<label>{label}</label>
			<div className="stepper">
				<button
					type="button"
					onClick={() => onChange(value - 1)}
					disabled={value <= min}
					aria-label={`Remove ${label.toLowerCase()}`}
				>
					&minus;
				</button>
				<span className="stepper-value">{value}</span>
				<button
					type="button"
					onClick={() => onChange(value + 1)}
					disabled={value >= max}
					aria-label={`Add ${label.toLowerCase()}`}
				>
					+
				</button>
			</div>
		</div>
	);
}
