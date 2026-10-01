interface MatrixInputProps {
	matrix: string[][];
	allowBlanks: boolean;
	onChange: (matrix: string[][]) => void;
}

export function MatrixInput({ matrix, allowBlanks, onChange }: MatrixInputProps) {
	const colCount = matrix[0]?.length ?? 0;

	const setCell = (row: number, col: number, value: string) => {
		const next = matrix.map((r) => [...r]);
		next[row][col] = value;
		onChange(next);
	};

	return (
		<div className="matrix" style={{ gridTemplateColumns: `repeat(${colCount}, auto)` }}>
			{matrix.map((row, r) =>
				row.map((cell, c) => (
					<input
						key={`${r}-${c}`}
						type="number"
						step="any"
						inputMode="decimal"
						value={cell}
						placeholder={allowBlanks ? '?' : '0'}
						aria-label={`Row ${r + 1}, column ${c + 1}`}
						onChange={(e) => setCell(r, c, e.target.value)}
					/>
				)),
			)}
		</div>
	);
}
