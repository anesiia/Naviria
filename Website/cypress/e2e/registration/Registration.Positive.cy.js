describe('Сторінка "Реєстрація"', () => {
  it('успішно реєструє нового користувача зі сторінки входу', () => {
    cy.visit('http://localhost:5173/login');

    cy.contains('Ще не з нами? Реєстрація').click();

    // Заповнення форми реєстрації
    cy.get('input[name="name"]').type('Марія');
    cy.get('input[name="surname"]').type('Тестова');
    cy.get('input[name="email"]').type(`testuser${Date.now()}@example.com`); // Унікальна пошта

    cy.get('input[name="password"]').type('StrongPass123');
    cy.get('input[name="repeat-password"]').type('StrongPass123');
    cy.get('input[name="nickname"]').type(`TUser${Date.now()}p`);
    cy.get('input[name="birth-date"]').type('2000-01-01');
    cy.get('input[name="gender"][value="F"]').check();

    cy.get('.submit-button').click();

    cy.url().should('include', '/profile');

    cy.visit("/achievements");

    // Перевірка, що є ачівка з назвою та описом
    cy.contains('.achievement .name', 'Registration').should('exist');

  });
});