-- 極光烈姫 セラス・ピュアライト

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
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

function OnPlaceCost(check)
	if check then return obj.CanCB(1) and obj.CanSpecificSB({q.Location, l.Soul, q.NameContains, obj.LoadName("Seraph"), q.Count, 1}) end
	obj.CounterBlast(1)
	obj.SpecificSoulBlast({q.Location, l.Soul, q.NameContains, obj.LoadName("Seraph"), q.Count, 1})
end

function OnPlaceCanFullyResolve()
	return obj.HasPrison()
end

function OnPlace()
	local hand = obj.EnemySelect({q.Location, l.EnemyHand, q.Count, 2})
	local soul = obj.EnemySelect({q.Location, l.EnemySoul, q.Count, 2})
	local rearguards = obj.EnemySelect({q.Location, l.EnemyRC, q.Count, 2})
	obj.StoreMultiple(hand, soul, rearguards)
	obj.Imprison({q.Location, l.Stored})
end

function Cont()
	if obj.IsPlayerTurn() then
		local count = math.floor(obj.GetNumberOf({q.Location, l.PlayerPrisoners}) / 2)
		obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, count * 5000, p.Continuous)
		if count >= 10 then
			obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusCritical, 1, p.Continuous)
		end
	end
end