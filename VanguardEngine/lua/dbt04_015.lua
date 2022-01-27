-- 彷徨の獄竜

function RegisterAbilities()
	-- on order played
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrderPlayed)
	ability1.SetLocation(l.Drop)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.NormalOrder, q.Other, o.Player})
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.Store(obj.SuperiorCall({q.Other, o.ThisFieldID}))
	if obj.Exists({q.Location, l.Stored}) then
		local option
		if obj.WasAlchemagic() then
			option = 3
		else
			option = obj.SelectOption(obj.GetDescription(2), obj.GetDescription(3))
		end
		if option == 1 or option == 3 then
			obj.AddCardValue({q.Location, l.Stored}, cs.BonusPower, 10000, p.UntilEndOfTurn)
		end
		if option == 2 or option == 3 then
			obj.AddCardValue({q.Location, l.Stored}, cs.BonusCritical, 1, p.UntilEndOfTurn)
		end
	end
end