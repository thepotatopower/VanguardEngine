-- Lovable Dress, Lija

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerUnits, q.NameContains, "Earnescorrect", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBattleEnds, p.HasPrompt, p.CB, 1, p.SendToBottom, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() then
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

function Activate(n)
	if n == 1 then
		obj.Draw(1)
		obj.ChooseAddTempPower(2, 5000)
	end
	return 0
end