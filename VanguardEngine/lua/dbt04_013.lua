-- 樹角獣帝 マグノリア・エルダー

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetActivation("OnPlace")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.VC)
	ability2.SetActivation("Cont")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlace()
	obj.SuperiorCall({q.Location, l.Soul, q.Count, 1, q.Min, 0})
end

function Cont()
	if obj.Exists({q.Location, l.Soul, q.Location, l.PlayerRC, q.NameContains, obj.LoadName("Magnolia"), q.Count, 1}) then
		obj.AddCardState({q.Location, l.PlayerRC}, cs.CanAttackFromBackRow, p.Continuous)
		obj.AddCardState({q.Location, l.PlayerRC}, cs.CanInterceptFromBackRow, p.Continuous)
		obj.AddCardValue({q.Location, l.PlayerRC}, cs.BonusPower, 5000, p.Continuous)
	end
end