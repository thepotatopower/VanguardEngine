-- パンデモニウム・タクティクス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.SoulCharge(4)
	if obj.InFinalRush() then
		if obj.SoulCount() >= 6 then 
			obj.Draw(1)
		end
		if obj.SoulCount() >= 8 then
			obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 10000, p.UntilEndOfTurn)
		end
		if obj.SoulCount() >= 12 then
			obj.Select({q.Location, l.PlayerVC, q.Count, 1})
			obj.AddCardValue({q.Location, l.Selected}, cs.BonusCritical, 1, p.UntilEndOfTurn)
		end
	end
end