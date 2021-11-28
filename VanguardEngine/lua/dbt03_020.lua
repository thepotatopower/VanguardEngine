-- イーズロッド・エンジェル

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
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function Activation()
	obj.LookAtTopOfDeck(1)
	if obj.IsSameZone() and not obj.Exists({q.Location, l.Looking, q.UnitType, u.NotUnit}) then
		local column = obj.GetColumn()
		obj.SuperiorCallToSpecificCircle({q.Location, l.Looking, q.Count, 1, q.Min, 0}, column)
	end
end