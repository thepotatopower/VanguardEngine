-- 樹角獣 パンテーロ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.CanSuperiorCallToSpecificCircle({q.Location, l.Drop, q.Name, obj.GetNameFromCardID("dbt04_028"), q.Count, 1}, FL.OpenCircle)
end

function OnPlace()
	obj.SuperiorCallToSpecificCircle({q.Location, l.Drop, q.Name, obj.GetNameFromCardID("dbt04_028"), q.Count, 1}, FL.OpenCircle)
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Magnolia")})
end

function OnAttack()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfBattle)
end