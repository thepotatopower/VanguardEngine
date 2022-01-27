-- 断裁の剣舞 チェーグラ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.CostNotRequired)
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.SoulCount() >= 7
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(1) end
	obj.CounterBlast(1) 
	obj.Discard(1)
end

function Activation()
	local count = obj.SoulCount()
	if count >= 7 and obj.ChoosesToPayCost() then
		obj.ChooseAddToHand({q.Location, l.Soul, q.Count, 1})
		obj.SoulCharge(1)
	end
	if count >= 13 then
		obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end

