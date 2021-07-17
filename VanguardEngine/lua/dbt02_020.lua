-- Soaring Dragon, Prideful Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Count, 4
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(1) and obj.CanCB(2) and obj.CanSB(3) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(2)
		obj.SoulBlast(3)
	end
end

function Activate(n)
	local selection = 1
	if n == 1 then
		if obj.PersonaRode() then
			selection = obj.SelectOption("Draw 1 card.", "This unit gets Power +5000 and Critical +1 until end of turn.")
		end
		if selection == 1 then
			obj.Draw(1)
		else
			obj.AddTempPower(4, 5000)
			obj.AddCritical(4, 1)
		end
	end
	return 0
end