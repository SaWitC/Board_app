import { AddBoardItemDTO } from '../models/board-item/add-board-item-DTO.interface';
import { UpdateBoardItemDTO } from '../models/board-item/update-board-item-DTO.interface';
import { BoardItemDetailsDTO } from '../models/board-item/board-item-details-DTO.interface';
import { BoardItemLookupDTO } from '../models/board-item/board-item-lookup-DTO.interface';

const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';

// export function boardItemToTask(source: BoardItemDetailsDTO | BoardItemLookupDTO): BoardItem {
// 	return {
// 		id: String(source.id),
// 		title: source.title,
// 		description: (source as any).description,
// 		boardColumnId: String((source as any).boardColumnId ?? ''),
// 		priority: source.priority,
// 		assignee: undefined,
// 		createdAt: new Date((source as any).createdTime ?? Date.now()),
// 		updatedAt: new Date((source as any).modificationDate ?? Date.now()),
// 		dueDate: (source as any).dueDate ? new Date((source as any).dueDate) : undefined,
// 		tags: [],
// 		taskType: source.taskType,
// 	};
// }

export function taskToCreateDto(task: Partial<AddBoardItemDTO>): AddBoardItemDTO {
	return {
		title: task.title ?? '',
		description: task.description ?? '',
		boardColumnId: task.boardColumnId ?? '',
		priority: task.priority!,
		assignee: EMPTY_GUID,
		dueDate: task.dueDate ? task.dueDate: null,
		taskType: task.taskType!,
	};
}

export function taskToUpdateDto(existing: BoardItemDetailsDTO, updates: Partial<UpdateBoardItemDTO>): UpdateBoardItemDTO {
	return {
		id: existing.id,
		title: updates.title ?? existing.title,
		description: updates.description ?? existing.description,
		boardColumnId: updates.boardColumnId ?? existing.boardColumnId,
		priority: updates.priority ?? existing.priority,
		assignee: existing.assignee ?? EMPTY_GUID,
		dueDate: updates.dueDate ? updates.dueDate: existing.dueDate,
		taskType: updates.taskType ?? existing.taskType,
	};
}
