-- 死招きの黒魔術

function RegisterAbilities()
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetGetCosts("GetCosts")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function GetCosts()
	local bind = obj.GetNumberOf({q.Location, l.Bind})
	return p.CB, 4 - bind
end

function Cost(check)
	local bind = obj.GetNumberOf({q.Location, l.Bind})
	if check then return obj.CanCB(4 - bind) end
	obj.CounterBlast(4 - bind)
end

function Activation()
	obj.Draw(2)
end