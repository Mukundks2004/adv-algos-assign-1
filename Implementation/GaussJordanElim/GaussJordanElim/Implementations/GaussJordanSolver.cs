using GaussJordanElim.Abstractions;

namespace GaussJordanElim.Implementations;

internal class GaussJordanSolver : IMatrixSolver
{
	public T[] Solve<T>(T[,] matrix) where T : IMatrixEntry<T>
	{
		// The pivot column needs to be stored outside the main elimination loop since it depends
		// on the last pivot column from previous elimination rounds

		int pivotColumnIndex = 0;
		int rowCount = matrix.GetLength(0);
		int colCount = matrix.GetLength(1);

		// 1) The elimination process occurs row by row. We move down rows and never backtrack.
		// With each row, the goal is to firstly give the row a leading 1, secondly make every
		// other row with the same column (as the leading 1) a 0.
		for (int rowToGiveLeading1Index = 0; rowToGiveLeading1Index < rowCount; rowToGiveLeading1Index++)
		{
			// This 'if' statement coupled with the increment at the end of the main loop is the
			// main driver of the pivot column. By default, after clearing a row, the pivot must
			// move 1 to the right. In the case of free variables, the pivot may move multiple times
			// to the right. But it will always move at least once per eliminated row.
			if (pivotColumnIndex >= colCount)
			{
				break;
			}

			// 2) The second step is to scan until we find the first non zero entry in the column.
			// Optimistically, this happens immediately- every new row we move one column across due
			// to the previously mentioned decrement and the number of independent variables is equal
			// to the amount of independent equations and we get a nice identity matrix. In practice, 
			// despite this being the case we may land on a zero which would inhibit the way the algorithm
			// works, we will not be able to use elementary row operations (ERO) to manipulate the matrix
			// in a meaningful way.
			//
			// Lets say we encounter a 0 where we don't expect one, perhaps this equation just has '0 x'.
			// Then we keep searching downwards to find a row with a non zero entry in this column, and then we
			// will swap it with this row. Remember that mathematically this doesn't change the system of 
			// equations. If we reach the end of the row and still no luck, it is still an outcome we anticipate
			// as only the previous equations may have included non zero quantities of the variable 'x'. But this
			// is unforunately not useful to perform ERO. So we reset the row searching index to the current row
			// and shift the column to the right and continue. If we make it to the end of the matrix without another
			// non zero entry, we decrement the pivotColumnIndex so that the swap function (#3 )is functionally a
			// no-op and the algorithm will terminate.
			int candidateNonZeroCellRowIndex = rowToGiveLeading1Index;
			while (matrix[candidateNonZeroCellRowIndex, pivotColumnIndex].IsZero())
			{
				candidateNonZeroCellRowIndex++;
				if (candidateNonZeroCellRowIndex == rowCount)
				{
					candidateNonZeroCellRowIndex++;
					pivotColumnIndex++;

					if (pivotColumnIndex == colCount)
					{
						pivotColumnIndex--;
						break;
					}
				}
			}

			// 3) If we have found a non zero row further down than the immediate row, we swap it with the immediate
			// row to get that nice leading 1 to enable row echelon form (REF).
			int rowToBeSwappedWithCurrentRowToGetLeading1Index = candidateNonZeroCellRowIndex;
			for (int columnIndex = 0; columnIndex < colCount; columnIndex++)
			{
				(matrix[rowToBeSwappedWithCurrentRowToGetLeading1Index, columnIndex], matrix[rowToGiveLeading1Index, columnIndex]) =
					(matrix[rowToGiveLeading1Index, columnIndex], matrix[rowToBeSwappedWithCurrentRowToGetLeading1Index, columnIndex]);
			}

			// 4) The juicy part: first divide this entire row by the leading cell value (to normalize it). Then for
			// every other cell n in the same column on row r, subtract r times n from r. This will zero the cell n 
			// while preserving the consistency of the system of linear equations given by the matrix.
			//
			// This is provided the algorithm doesn't terminate due to only zeroes in all cells in below rows.
			// If we didn't find any non zero cells, the row to be eliminated will be the first zero row.
			T currentCell = matrix[rowToGiveLeading1Index, pivotColumnIndex];

			if (!currentCell.IsZero())
			{
				// By the time rowToBeEliminatedIndex becomes a given value, every column before the pivotColumnIndex
				// in that row has been used as a pivot column in an earlier iteration of the outer loop.
				for (int cellToNormalizeIndex = pivotColumnIndex; cellToNormalizeIndex < colCount; cellToNormalizeIndex++)
				{
					matrix[rowToGiveLeading1Index, cellToNormalizeIndex] = matrix[rowToGiveLeading1Index, cellToNormalizeIndex] / currentCell;
				}

				for (int rowToZeroIndex = 0; rowToZeroIndex < rowCount; rowToZeroIndex++)
				{
					// We don't want to zero the current row
					if (rowToZeroIndex != rowToGiveLeading1Index)
					{

					}
				}
			}


			pivotColumnIndex++;
		}

		throw new NotImplementedException();
	}
}
