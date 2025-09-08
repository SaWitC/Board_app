export interface BoardColumnDTO {
  title: string;
  description: string;
}

export interface AddBoardDTO {
  title: string;
  description: string;
  users: string[];
  admins: string[];
  owners: string[];
  boardColumns: BoardColumnDTO[];
}
