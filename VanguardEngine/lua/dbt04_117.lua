-- 樹角獣 レムレア

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.FrontRowRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.Applicable, q.Other, o.NotThis}) 
end

function Condition()
	return obj.IsSameZone()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	if obj.CanPayCost("Cost") and obj.PayAdditionalCost() then
		obj.PayCost("Cost")
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end