-- グラビディア・ネルトリンガー

function RegisterAbilities()
	-- placed on VC
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("PlacedOnVCTrigger")
	ability1.SetActivation("PlacedOnVC")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.PlayerVC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
end

function PlacedOnVCTrigger()
	return obj.LastPlacedOnVC() 
end

function PlacedOnVC()
	obj.ChoosePutIntoOrderZone({q.Location, l.Deck, q.Other, o.Meteorite, q.Count, 2, q.Min, 0})
	obj.Shuffle()
end

function OnAttackTrigger()
	return obj.IsAttackingUnit() 
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) and obj.CanAddToDrop({q.Location, l.Order, q.Other, o.Meteorite, q.Count, 1}) end
	obj.CounterBlast(1)
	obj.Store(obj.ChooseAddToDrop({q.Location, l.Order, q.Other, o.Meteorite, q.Min, 1}))
end

function OnAttack()
	if obj.IsSameZone() then
		obj.AddCardValue({q.Location, l.PlayerVC, q.Other, o.This}, cs.BonusPower, 15000, p.UntilEndOfBattle)
		if obj.Exists({q.Location, l.Stored, q.Count, 3}) then
			obj.AddCardValue({q.Location, l.PlayerVC, q.Other, o.This}, cs.BonusCritical, 1, p.UntilEndOfBattle)
		end
		if obj.Exists({q.Location, l.Stored, q.Count, 5}) then
			obj.AddPlayerState(ps.DoubleTriggerEffects, p.UntilEndOfBattle)
		end
	end
end
