export interface BoardColumnDTO {
  title: string;
  description: string;
}

export interface BoardUserDTO {
  email: string;
  role: number;
}

export interface AddBoardDTO {
  title: string;
  description: string;
  boardUsers: BoardUserDTO[];
  boardColumns: BoardColumnDTO[];
}
