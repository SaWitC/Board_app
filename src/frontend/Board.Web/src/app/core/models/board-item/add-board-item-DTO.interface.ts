import { TaskType } from "../enums/task-type.enum";

export interface AddBoardItemDTO {
  title: string;
  description: string;
  boardColumnId: string;
  priority: number;
  assigneeId: string;
  dueDate: string;
  taskType: TaskType;
}
