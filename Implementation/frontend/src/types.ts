export type SolveType = 'GaussJordan' | 'Lu' | 'MagicSquare';

export interface SolveRequest {
	solveType: SolveType;
	matrix: (number | null)[][];
}

export interface SolveStep {
	state: number[][];
	description: string;
}

export const SOLVE_TYPES: { value: SolveType; label: string }[] = [
	{ value: 'GaussJordan', label: 'Gauss-Jordan Elimination' },
	{ value: 'Lu', label: 'LU Decomposition' },
	{ value: 'MagicSquare', label: 'Magic Square' },
];
