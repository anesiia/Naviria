describe('Сторінка "Досягнення"', () => {
    it('відображає досягнення для Алекса (авторизований користувач із досягненнями)', () => {
        cy.loginAsAlex();
        cy.visit('/achievements');

        cy.contains('Твої досягнення').should('be.visible');
        cy.get('.achievement').should('have.length.at.least', 1);
        cy.get('.gift-btn').first().should('contain', 'Подарунок');
    });

    it('не відображає досягнень для Марії (авторизований користувач без досягнень)', () => {
        cy.loginAsNataliya();
        cy.visit('/achievements');

        cy.contains('Твої досягнення').should('be.visible');

        cy.get('.achievement').should('have.length', 0);

        cy.contains('Немає досягнень').should('be.visible');
    });
});

