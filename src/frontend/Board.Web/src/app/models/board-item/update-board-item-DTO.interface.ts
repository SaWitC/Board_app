export interface UpdateBoardItemDTO {
  id: string;
  title: string;
  description: string;
  boardColumnId: string;
  priority: number;
  assigneeId: string;
  dueDate: string;
}
