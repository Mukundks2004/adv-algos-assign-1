import { useCallback, useMemo, useState } from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { ApiError, solveMatrix } from './api';
import { MatrixInput } from './components/MatrixInput';
import { Modal } from './components/Modal';
import { Stepper } from './components/Stepper';
import { StepViewer } from './components/StepViewer';
import gaussJordanDoc from './docs/gauss-jordan.md?raw';
import luDoc from './docs/lu.md?raw';
import magicSquareDoc from './docs/magic-square.md?raw';
import { SOLVE_TYPES, type SolveStep, type SolveType } from './types';

const DOCS: Record<SolveType, string> = {
	GaussJordan: gaussJordanDoc,
	Lu: luDoc,
	MagicSquare: magicSquareDoc,
};

const MIN_SIZE = 1;
const MAX_SIZE = 8;

function buildMatrix(source: string[][], rows: number, cols: number): string[][] {
	return Array.from({ length: rows }, (_, r) =>
		Array.from({ length: cols }, (_, c) => source[r]?.[c] ?? ''),
	);
}

function App() {
	const [solveType, setSolveType] = useState<SolveType>('GaussJordan');
	const [matrix, setMatrix] = useState<string[][]>(() => buildMatrix([], 3, 4));
	const [infoOpen, setInfoOpen] = useState(false);
	const [loading, setLoading] = useState(false);
	const [steps, setSteps] = useState<SolveStep[] | null>(null);
	const [solveCount, setSolveCount] = useState(0);
	const [error, setError] = useState<string | null>(null);

	const rows = matrix.length;
	const cols = matrix[0]?.length ?? 0;
	const isMagicSquare = solveType === 'MagicSquare';

	const solverLabel = useMemo(
		() => SOLVE_TYPES.find((option) => option.value === solveType)?.label ?? solveType,
		[solveType],
	);

	const setRows = (next: number) => setMatrix((m) => buildMatrix(m, next, cols));
	const setCols = (next: number) => setMatrix((m) => buildMatrix(m, rows, next));

	// A magic square must be square, so changing either dimension resizes both.
	const setSquareSize = (next: number) => setMatrix((m) => buildMatrix(m, next, next));

	const handleSolveTypeChange = (next: SolveType) => {
		setSolveType(next);
		setSteps(null);

		if (next === 'MagicSquare' && rows !== cols) {
			setMatrix((m) => buildMatrix(m, rows, rows));
		}
	};

	const handleSolve = useCallback(async () => {
		setLoading(true);
		setError(null);

		const parsed = matrix.map((row) =>
			row.map((cell) => (cell.trim() === '' ? null : Number(cell))),
		);

		try {
			const result = await solveMatrix({ solveType, matrix: parsed });
			setSteps(result);
			setSolveCount((count) => count + 1);
		} catch (err) {
			setSteps(null);
			setError(err instanceof ApiError ? err.message : 'An unexpected error occurred.');
		} finally {
			setLoading(false);
		}
	}, [matrix, solveType]);

	return (
		<div className="page">
			<header className="page-header">
				<h1>Matrix Solver</h1>
				<button
					type="button"
					className="icon-btn"
					onClick={() => setInfoOpen(true)}
					aria-label={`About ${solverLabel}`}
					title={`About ${solverLabel}`}
				>
					i
				</button>
			</header>
			<p className="subtitle">Step through each operation the algorithm performs.</p>

			<div className="card">
				<h2>Setup</h2>
				<div className="field-row">
					<div className="field">
						<label htmlFor="solver">Algorithm</label>
						<select
							id="solver"
							value={solveType}
							onChange={(e) => handleSolveTypeChange(e.target.value as SolveType)}
						>
							{SOLVE_TYPES.map((option) => (
								<option key={option.value} value={option.value}>
									{option.label}
								</option>
							))}
						</select>
					</div>

					{isMagicSquare ? (
						<Stepper
							label="Size"
							value={rows}
							min={MIN_SIZE}
							max={MAX_SIZE}
							onChange={setSquareSize}
						/>
					) : (
						<>
							<Stepper label="Rows" value={rows} min={MIN_SIZE} max={MAX_SIZE} onChange={setRows} />
							<Stepper
								label="Columns"
								value={cols}
								min={MIN_SIZE}
								max={MAX_SIZE}
								onChange={setCols}
							/>
						</>
					)}
				</div>
			</div>

			<div className="card">
				<h2>{isMagicSquare ? 'Square' : 'Augmented matrix'}</h2>
				<MatrixInput matrix={matrix} allowBlanks={isMagicSquare} onChange={setMatrix} />
				<p className="hint">
					{isMagicSquare
						? 'Leave a cell empty to mark it as an unknown to solve for.'
						: 'The final column is the right-hand side of the system.'}
				</p>
			</div>

			<button type="button" className="btn" onClick={handleSolve} disabled={loading}>
				{loading ? 'Solving…' : 'Solve'}
			</button>

			{steps && (
				<div style={{ marginTop: 20 }}>
					<StepViewer key={solveCount} steps={steps} />
				</div>
			)}

			<Modal open={infoOpen} title={solverLabel} onClose={() => setInfoOpen(false)}>
				<ReactMarkdown remarkPlugins={[remarkGfm]}>{DOCS[solveType]}</ReactMarkdown>
			</Modal>

			<Modal
				open={error !== null}
				title="Could not solve"
				size="sm"
				onClose={() => setError(null)}
				footer={
					<button type="button" className="btn btn-secondary" onClick={() => setError(null)}>
						Dismiss
					</button>
				}
			>
				<p className="error-text">{error}</p>
			</Modal>
		</div>
	);
}

export default App;
