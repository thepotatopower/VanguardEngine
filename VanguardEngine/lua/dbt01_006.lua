-- Aurora Battle Princess, Agra Rouge

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerPrisoners, q.Count, 2
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 4 then
		return q.Location, l.FrontRowEnemyRC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 2 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() and obj.CanSB(3) and obj.HasPrison() and obj.Exists(1) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.SoulBlast(3)
	end
end

function Activate(n)
	if n == 1 then
		if obj.Exists(1) then
			obj.SetAbilityPower(2, 5000)
			obj.SetAbilityShield(2, 10000)
		else
			obj.SetAbilityPower(2, 0)
			obj.SetAbilityShield(2, 0)
		end
	elseif n == 2 then
		obj.ChooseImprison(4)
	end
	return 0
end