
describe('Сторінка "Реєстрація"', () => {
  it('успішно реєструє нового користувача зі сторінки входу', () => {
    cy.visit('http://localhost:5173/login');

    // Перехід на сторінку реєстрації
    cy.contains('Ще не з нами? Реєстрація').click();

    // Заповнення форми реєстрації
    cy.get('input[name="name"]').type('Марія');
    cy.get('input[name="surname"]').type('Тестова');
    cy.get('input[name="email"]').type(`testuser${Date.now()}@example.com`); // Унікальна пошта

    cy.get('input[name="password"]').type('StrongPass123');
    cy.get('input[name="repeat-password"]').type('StrongPass123');
    cy.get('input[name="nickname"]').type(`TestUser3000${Date.now()}p`);
    cy.get('input[name="birth-date"]').type('2000-01-01');
    cy.get('input[name="gender"][value="F"]').check();

    // Відправка форми
    cy.get('.submit-button').click();

    // Очікується редірект або повідомлення про успішну реєстрацію
    cy.url().should('include', '/profile');
    cy.contains('Особистий прогрес').should('exist');
  });
});