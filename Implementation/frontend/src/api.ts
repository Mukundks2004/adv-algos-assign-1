import type { SolveRequest, SolveStep } from './types';

const API_BASE_URL = (import.meta.env.VITE_API_BASE_URL as string | undefined) ?? 'http://localhost:5070';

export class ApiError extends Error {}

export async function solveMatrix(request: SolveRequest): Promise<SolveStep[]> {
	let response: Response;

	try {
		response = await fetch(`${API_BASE_URL}/Solve/GetSteps`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify(request),
		});
	} catch {
		throw new ApiError('Could not reach the solver API. Is the backend running?');
	}

	if (!response.ok) {
		const message = await response.text();
		throw new ApiError(message || `Request failed with status ${response.status}`);
	}

	return (await response.json()) as SolveStep[];
}
