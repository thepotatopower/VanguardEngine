-- オーリエイトヘイズ・ラプチャー

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_003")})
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.Hand, q.Count, 1}) end
	obj.ChooseAddToSoul({q.Location, l.Hand, q.Count, 1})
end

function Activation()
	local power
	if obj.SoulCount() >= 10 then
		power = 10000
	else
		power = 5000
	end
	obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, power, p.UntilEndOfTurn)
end