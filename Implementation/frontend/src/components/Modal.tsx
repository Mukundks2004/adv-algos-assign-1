import { useEffect, type ReactNode } from 'react';

interface ModalProps {
	open: boolean;
	title: string;
	onClose: () => void;
	children: ReactNode;
	footer?: ReactNode;
	size?: 'sm' | 'md';
}

export function Modal({ open, title, onClose, children, footer, size = 'md' }: ModalProps) {
	useEffect(() => {
		if (!open) {
			return;
		}

		const onKeyDown = (e: KeyboardEvent) => e.key === 'Escape' && onClose();
		window.addEventListener('keydown', onKeyDown);
		return () => window.removeEventListener('keydown', onKeyDown);
	}, [open, onClose]);

	if (!open) {
		return null;
	}

	return (
		<div className="overlay" onClick={onClose} role="presentation">
			<div
				className={size === 'sm' ? 'modal modal-sm' : 'modal'}
				onClick={(e) => e.stopPropagation()}
				role="dialog"
				aria-modal="true"
				aria-label={title}
			>
				<div className="modal-header">
					<h2>{title}</h2>
					<button type="button" className="icon-btn" onClick={onClose} aria-label="Close">
						&times;
					</button>
				</div>
				<div className="modal-body">{children}</div>
				{footer && <div className="modal-footer">{footer}</div>}
			</div>
		</div>
	);
}
