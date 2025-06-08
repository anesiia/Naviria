describe('Сторінка "Вхід"', () => {
  it('успішний вхід з правильними обліковими даними', () => {
    cy.visit('/login');

    cy.get('input[name="email"]').type('alexander.davis@example.com');
    cy.get('input[name="password"]').type('Alex1234');
    cy.get('.submit-button').click();

    cy.url().should('include', '/profile');
    cy.get('p.naming', { timeout: 10000 }).should('exist');
  });
});

describe('Сторінка входу: неіснуючий користувач', () => {
  it('вхід не виконується з неправильними обліковими даними', () => {
    cy.visit('/login');

    cy.get('input[name="email"]').type('nonexistent.user@example.com');
    cy.get('input[name="password"]').type('WrongPassword123');
    cy.get('.submit-button').click();

    cy.url().should('include', '/login');
  });
});
