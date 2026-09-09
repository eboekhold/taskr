import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/TaskrClient/);
});

test('home page shows task not found message', async ({ page }) => {
  await page.goto('');

  await expect(page.getByText(/task not found/)).toBeVisible();
})

test('tasks link shows tasks header', async ({ page }) => {
  await page.goto('');

  // Click the Tasks link.
  await page.getByRole('link', { name: 'Tasks' }).click();

  // Expect page to have a message saying no tasks found.
  await expect(page.getByRole('heading', { name: 'Tasks' })).toBeVisible();
});
