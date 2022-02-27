-- 想望の魔法 エスノノ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.VanguardIs(obj.GetNameFromCardID("dbt01_008")) or obj.VanguardIs(obj.GetNameFromCardID("dbt01_046"))
end

function Cost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function Activation()
	obj.LookAtTopOfDeck(2)
	obj.Select({q.Location, l.Looking, q.Count, 2, q.Min, 0})
	obj.RearrangeOnTop({q.Location, l.Selected})
	obj.RearrangeOnBottom({q.Location, l.Looking})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end