-- 潮風攫い

function RegisterAbilities()
	-- on attack hits
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("OnAttackHitsTrigger")
	ability1.SetCost("OnAttackHitsCost")
	ability1.SetCanFullyResolve("OnAttackHitsCanFullyResolve")
	ability1.SetActivation("OnAttackHits")
end

function OnAttackHitsTrigger()
	return obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked}) and obj.IsBooster()
end

function OnAttackHitsCost(check)
	if check then return obj.CanBind({q.Location, l.Drop, q.Other, o.Order, q.Count, 1}) end
	obj.ChooseBind({q.Location, l.Drop, q.Other, o.Order, q.Count, 1})
end

function OnAttackHitsCanFullyResolve()
	return obj.Exists({q.Location, l.Drop, q.Other, o.Order, q.Count, 2})
end

function OnAttackHits()
	obj.ChooseAddToHand({q.Location, l.Drop, q.Other, o.Order, q.Count, 1})
end