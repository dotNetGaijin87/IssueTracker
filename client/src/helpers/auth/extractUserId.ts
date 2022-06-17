function extractUserId(user: string | undefined): string {
  if (user === undefined) {
    new Error('No UserId');
  }
  let userId = user?.split('|').pop();

  if (userId === undefined) {
    new Error('No UserId');
  }
  return userId!;
}

export default extractUserId;
