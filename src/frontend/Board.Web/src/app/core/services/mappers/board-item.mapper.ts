import { BoardItem } from '../../models';
import { AddBoardItemDTO } from '../../models/board-item/add-board-item-DTO.interface';
import { UpdateBoardItemDTO } from '../../models/board-item/update-board-item-DTO.interface';
import { BoardItemDetailsDTO } from '../../models/board-item/board-item-details-DTO.interface';
import { BoardItemLookupDTO } from '../../models/board-item/board-item-lookup-DTO.interface';
import { TaskPriority } from '../../models/enums/task-priority.enum';

const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';

export function mapPriorityToNumber(priority: TaskPriority | undefined): number {
	if (priority === TaskPriority.LOW) return 0;
	if (priority === TaskPriority.MEDIUM) return 1;
	if (priority === TaskPriority.HIGH) return 2;
	if (priority === TaskPriority.URGENT) return 3;
	return 1;
}

export function mapPriorityFromNumber(value: number | string | undefined): TaskPriority {
	const n = typeof value === 'string' ? Number(value) : value;
	switch (n) {
		case 0: return TaskPriority.LOW;
		case 1: return TaskPriority.MEDIUM;
		case 2: return TaskPriority.HIGH;
		case 3: return TaskPriority.URGENT;
		default: return TaskPriority.MEDIUM;
	}
}

export function boardItemToTask(source: BoardItemDetailsDTO | BoardItemLookupDTO): BoardItem {
	return {
		id: String(source.id),
		title: source.title,
		description: (source as any).description,
		columnId: String((source as any).boardColumnId ?? ''),
		priority: mapPriorityFromNumber((source as any).priority),
		assignee: undefined,
		createdAt: new Date((source as any).createdTime ?? Date.now()),
		updatedAt: new Date((source as any).modificationDate ?? Date.now()),
		dueDate: (source as any).dueDate ? new Date((source as any).dueDate) : undefined,
		tags: [],
		taskType: source.taskType,
	};
}

export function taskToCreateDto(task: Partial<BoardItem>): AddBoardItemDTO {
	return {
		title: task.title ?? '',
		description: task.description ?? '',
		boardColumnId: task.columnId ?? '',
		priority: mapPriorityToNumber(task.priority),
		assigneeId: EMPTY_GUID,
		dueDate: task.dueDate ? new Date(task.dueDate).toISOString() : new Date().toISOString(),
		taskType: task.taskType!,
	};
}

export function taskToUpdateDto(existing: BoardItemDetailsDTO, updates: Partial<BoardItem>): UpdateBoardItemDTO {
	return {
		id: existing.id,
		title: updates.title ?? existing.title,
		description: updates.description ?? existing.description,
		boardColumnId: updates.columnId ?? existing.boardColumnId,
		priority: updates.priority ? mapPriorityToNumber(updates.priority) : (existing as any).priority,
		assigneeId: existing.assigneeId ?? EMPTY_GUID,
		dueDate: updates.dueDate ? new Date(updates.dueDate).toISOString() : existing.dueDate,
		taskType: updates.taskType ?? existing.taskType,
	};
}
