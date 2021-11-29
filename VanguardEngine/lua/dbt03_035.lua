-- デザイアデビル ムッカー

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetCost("OnPlaceCost")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_003")})
end

function OnPlaceCost(check)
	if check then return obj.CanCB(1) and obj.CanSB(2) end
	obj.CounterBlast(1)
	obj.SoulBlast(2)
end

function OnPlace()
	obj.SuperiorCall({q.Location, l.Soul, q.Count, 1})
end

function OnAttackTrigger()
	return obj.Exists({q.Location, l.PlayerVC, q.Other, o.Attacking})
end

function OnAttackCondition() 
	return obj.PersonaRode()
end

function OnAttack()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end