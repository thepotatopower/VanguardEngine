-- リムーバル・エンジェル

function RegisterAbilities()
	-- on place 
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanSpecificDiscard({q.Location, l.Hand, q.UnitType, u.Sentinel, q.Count, 1}) end
	obj.SpecificDiscard({q.Location, l.Hand, q.UnitType, u.Sentinel, q.Count, 1})
end

function Activation()
	local option = obj.SelectOption(obj.GetDescription(2), obj.GetDescription(3))
	if option == 1 then
		obj.CounterCharge(1)
	else
		obj.SoulCharge(2)
	end
end
